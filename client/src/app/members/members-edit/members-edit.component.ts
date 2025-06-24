import { Component, inject, OnInit } from '@angular/core';
import deepEqual from 'fast-deep-equal';
import { MembersService } from '../../services/members.service';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../../services/accounts.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Member } from '../../utils/models/member';
import { convertDateToLocale } from '../../utils/app.utils';
import { ToastrService } from 'ngx-toastr';
import { CanComponentDeactivate } from '../../utils/models/can-deactivate';
import rfdc from 'rfdc';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { AuthSignal } from '../../utils/models/auth-types';

@Component({
  selector: 'app-members-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, TabsModule, PhotoEditorComponent],
  templateUrl: './members-edit.component.html',
  styleUrl: './members-edit.component.css'
})
export class MembersEditComponent implements OnInit, CanComponentDeactivate {
  member?: Member;
  originalState?: Member;
  
  memberSvc = inject(MembersService);
  accountSvc = inject(AccountService);
  activatedRoute = inject(ActivatedRoute);
  toastr = inject(ToastrService);
  
  clone = rfdc();
  convertDate = convertDateToLocale

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.accountSvc.isLoggedIn()?.userName;
    if (username) {
      this.memberSvc.getMember(username).subscribe({
        next: (response) => {
          this.member = response;
          this.originalState = this.clone(response);
          this.accountSvc.refreshUser({
            userName: response.username,
            thumbnail: response.photos.find(ph => ph.isMain)?.url as string
          });
        },
        error: (error) => {
          console.error('Error loading member:', error);
        }
      });
    }
  }

  canDeactivate() {
    const hasChanges = !deepEqual(this.member, this.originalState);
    return hasChanges
      ? confirm('You have unsaved changes. Do you really want to leave?')
      : true;
  }

  saveMember(form: NgForm) {
    this.memberSvc.updateMember(this.member as Member)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.toastr.success('Profile updated successfully', 'Success');
          form.reset(this.member);
          this.originalState = this.clone(this.member);
        },
        error: (error) => {
          this.toastr.error("Failed to updated profile", "ERROR");
          form.reset(this.originalState); // Reverting Back
          console.log(error.error);
        }
      });
  }

  cancel() {
    // Logic to cancel the edit operation, e.g., navigate back or reset form
    console.log('Edit cancelled');
  }
}

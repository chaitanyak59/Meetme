import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../services/members.service';
import { Member } from '../../utils/models/member';
import { CardComponent } from '../card/card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-members-list',
  standalone: true,
  imports: [CardComponent, CommonModule],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css'
})
export class MembersListComponent implements OnInit {
  members: Member[] = [];
  membersSvc = inject(MembersService);

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.membersSvc.getMembers().subscribe({
      next: (response) => {
        this.members = response;
      },
      error: (error) => {
        console.error('Error loading members:', error);
      }
    });
  }

}

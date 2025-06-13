import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../utils/models/member';
import { convertDateToLocale } from '../../utils/app.utils';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-members-detail',
  standalone: true,
  imports: [CommonModule, TabsModule, GalleryModule],
  templateUrl: './members-detail.component.html',
  styleUrl: './members-detail.component.css'
})
export class MembersDetailComponent implements OnInit {
  memberSvc = inject(MembersService);
  activatedRoute = inject(ActivatedRoute);
  convertDateToLocale = convertDateToLocale
  member: Member | null = null;

  images: GalleryItem[] = [new ImageItem({
    src: '/assets/user.png',
    thumb: '/assets/user.png'
  })];

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.activatedRoute.snapshot.paramMap.get('name');
    if (username) {
      this.memberSvc.getMember(username).subscribe({
        next: (response) => {
          this.member = response;
          this.images = this.member.photos.map(photo => {
            return new ImageItem({
              src: photo.url,
              thumb: photo.url,
            })
          });
        },
        error: (error) => {
          console.error('Error loading member:', error);
        }
      });
    }
  }

}

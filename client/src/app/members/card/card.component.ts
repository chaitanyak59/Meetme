import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Member } from '../../utils/models/member';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
  @Input() member!: Member;

}

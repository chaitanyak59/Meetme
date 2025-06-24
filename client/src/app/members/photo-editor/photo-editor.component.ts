import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Member } from '../../utils/models/member';
import { CommonModule } from '@angular/common';
import { FileUploader } from 'ng2-file-upload';
import { PhotoUploaderComponent } from '../photo-uploader/photo-uploader.component';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [CommonModule, PhotoUploaderComponent],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent {
  @Input() member?: Member;
  @Output() onMemberProfileUpdated: EventEmitter<void> = new EventEmitter();

  public deleteProfilePhoto(publicID: number): void {
    console.log("Public ID:", publicID);
  }
}

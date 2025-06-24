import { CommonModule } from '@angular/common';
import { Component, EventEmitter, inject, Inject, Input, OnInit, Output } from '@angular/core';
import { FileItem, FileUploader, FileUploadModule, ParsedResponseHeaders } from 'ng2-file-upload';
import { APP_CONFIG } from '../../utils/config';
import { AccountService } from '../../services/accounts.service';

@Component({
  selector: 'app-photo-uploader',
  standalone: true,
  imports: [CommonModule, FileUploadModule],
  templateUrl: './photo-uploader.component.html',
  styleUrl: './photo-uploader.component.css'
})
export class PhotoUploaderComponent implements OnInit {
  @Input() uploadUrl = '/users/add-photo';
  @Output() onMemberProfileUpdated: EventEmitter<void> = new EventEmitter<void>();

  accountSvc = inject(AccountService);
  config = inject(APP_CONFIG);

  uploader?: FileUploader;


  public ngOnInit(): void {
    this.uploader = new FileUploader({
      url: this.config.apiUrl + this.uploadUrl,
      isHTML5: true,
      removeAfterUpload: true,
      autoUpload: false,
      allowedFileType: ['image'],
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onSuccessItem = this.onFileUploaded.bind(this);
    this.uploader.onBeforeUploadItem = this.onFileBeforeUpload.bind(this);
  }

  public hasBaseDropZoneOver = false;

  public onFileUploaded(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders): void {
    console.log({
      item,
      status,
      response
    });
    this.onMemberProfileUpdated.emit();
  }

  public onFileBeforeUpload(fileItem: FileItem): void {
    const token = localStorage.getItem("user.token");
    fileItem.withCredentials = false;
    fileItem.headers = [
      { name: 'Authorization', value: `Bearer ${token}` },
    ];
  }
}

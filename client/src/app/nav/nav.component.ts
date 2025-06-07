import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  @Output() loginSubmit: EventEmitter<{username: string, password: string}> = new EventEmitter();
  @Output() logoutSubmit: EventEmitter<void> = new EventEmitter();

  @Input() isLoggedIn: boolean = false;

  model: any = {
    userName: '',
    password: ''
  };

  login() {
    if (this.model.userName && this.model.password) {
      this.loginSubmit.emit({
        username: this.model.userName,
        password: this.model.password
      });
    } else {
      console.error('Login failed: Username and password are required.');
    }
  }

  logout() {
    this.logoutSubmit.emit();
    this.model.userName = '';
  }
}

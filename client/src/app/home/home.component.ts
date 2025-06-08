import { Component, inject } from '@angular/core';
import { RegisterFormComponent } from "../register-form/register-form.component";
import { AccountService } from '../services/accounts.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterFormComponent, RegisterFormComponent, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  isRegisterMode = false;
  accountSvc = inject(AccountService);

  toggleRegisterMode() {
    this.isRegisterMode = !this.isRegisterMode;
  }

  handleRegister(event: { username: string; password: string }) {
    this.accountSvc.registerUser({
      userName: event.username,
      password: event.password
    }).subscribe({
      error: (error: any) => {
        console.error('Registration failed:', error);
      }
    });
  }
}

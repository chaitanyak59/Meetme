import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterOutlet } from '@angular/router';
import { APP_CONFIG } from './utils/config';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './services/accounts.service';
import { UsersService } from './services/users.service';
import { CommonModule } from '@angular/common';
import { ToastNoAnimationModule, ToastrService } from 'ngx-toastr';

interface User {
  id: number;
  userName: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, NavComponent, CommonModule, ToastNoAnimationModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: []
})
export class AppComponent {
  accountSvc = inject(AccountService);
  usersSvc = inject(UsersService);
  router = inject(Router);
  toastr = inject(ToastrService);


  handleLogin(event: { username: string, password: string }): void {
    this.accountSvc.loginUser({
      userName: event.username,
      password: event.password
    }).subscribe({
      next: (response: any) => {
        this.toastr.success(`Welcome ${response.userName}!`, 'Succesfully logged in');
        this.router.navigateByUrl('/');
      },
      error: (error: any) => {
          this.toastr.error(`Incorrect Credentials`, 'Error');
      }
    });
  }

  handleLogout(): void {
    this.accountSvc.logoutUser();
    this.router.navigateByUrl('/');
    this.toastr.info("", 'Logged out!');
  }
}

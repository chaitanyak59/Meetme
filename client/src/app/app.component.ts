import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { APP_CONFIG } from './utils/config';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './services/accounts.service';
import { UsersService } from './services/users.service';
import { CommonModule } from '@angular/common';
import { HomeComponent } from "./home/home.component";

interface User {
  id: number;
  userName: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, NavComponent, CommonModule, HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: []
})
export class AppComponent implements OnInit {
  accountSvc  = inject(AccountService);
  usersSvc    = inject(UsersService);
  users: User[] = [];

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.usersSvc.getAllUsers().subscribe({
      next: (response: User[]) => {
        this.users = response;
      },
      error: (error: any) => {
        console.error('Failed to fetch users:', error);
      }
    });
  }

  handleLogin(event: { username: string, password: string }): void {
    this.accountSvc.loginUser({
      userName: event.username,
      password: event.password
    }).subscribe({
      next: (response: any) => {
        localStorage.setItem('user.token', response.token);
        this.getUsers();
      },
      error: (error: any) => {
        console.error('Login failed:', error);
      }
    });
  }

  handleLogout(): void {
    this.accountSvc.logoutUser();
    this.users = [];
  }

}

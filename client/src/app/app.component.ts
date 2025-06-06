import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

interface User {
  id: number;
  userName: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: []
})
export class AppComponent implements OnInit {
  http  = inject(HttpClient);
  title = 'Meet Me';
  users: User[] = [];

  ngOnInit(): void {
    this.http.get("http://localhost:5115/api/users")
          .subscribe((res: any) => {
            this.users = res;
        });
  }
}

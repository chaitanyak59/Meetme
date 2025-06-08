import { CommonModule, NgFor } from '@angular/common';
import { Component, output } from '@angular/core';
import { Form, FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-register-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css'
})
export class RegisterFormComponent {

  model = {};
  registrationCancelled = output<void>();
  registerEvent = output<{ username: string; password: string }>();

  register(form: NgForm) {
    if (form.invalid) return;

    const { username, password } = form.value;
    this.registerEvent.emit({ username, password });
  }

  cancel() {
    this.registrationCancelled.emit();
  }
}

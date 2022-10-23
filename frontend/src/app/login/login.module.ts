import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginViewComponent } from './login-view.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from "@angular/forms";
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
  declarations: [
    LoginViewComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,

    // Material
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
  ],
  exports: [
    LoginViewComponent,
  ]
})
export class LoginModule { }

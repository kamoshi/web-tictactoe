import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {IdentityService} from "../identity.service";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-login-view',
  template: `
    <h1>Choose your username</h1>

    <div class="login-form">
      <mat-form-field class="example-full-width" appearance="fill">
        <mat-label>Username</mat-label>
        <input matInput [(ngModel)]="username">
      </mat-form-field>

      <button mat-raised-button color="primary" (click)="onChoose()">Choose</button>
    </div>
  `,
  styles: [`
    h1 {
      margin-top: 2em;
      text-align: center;
    }
    .login-form {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 1em;
    }
  `],
})
export class LoginViewComponent {
  username = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private identity: IdentityService,
    private _snackBar: MatSnackBar,
  ) {}

  onChoose() {
    if (!this.username.length)
      return;

    this.http.post<number>('https://localhost:7135/api/Auth', { name: this.username })
      .subscribe({
        next: async id => {
          this.identity.save({ name: this.username, id });
          this._snackBar.open(`Logged in as ${this.username}`, undefined, { duration: 2000 });
          await this.router.navigate(['']);
        },
        error: err => console.error(err)
      })

  }

}

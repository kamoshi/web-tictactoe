import { Component } from '@angular/core';
import {IdentityService} from "./identity.service";

@Component({
  selector: 'app-root',
  template: `
    <mat-toolbar color="primary">
      <span>TicTacToe</span>
      <button mat-flat-button color="primary" routerLink="login">Login</button>
      <button mat-flat-button color="primary" routerLink="game">Game</button>
      <button mat-flat-button color="primary" routerLink="leaderboard">Leaderboard</button>

      <span class="username" *ngIf="identity">{{ identity.name }}</span>
    </mat-toolbar>
    <router-outlet></router-outlet>
  `,
  styles: [`
    .username {
      margin-left: auto;
    }
  `]
})
export class AppComponent {

  constructor(
    public identity: IdentityService
  ) {}

}

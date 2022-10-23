import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginViewComponent } from "./login/login-view.component";
import { LeaderboardViewComponent } from "./leaderboard/leaderboard-view.component";
import { GameViewComponent } from "./game/game-view.component";

const routes: Routes = [
  {
    path: 'login',
    component: LoginViewComponent,
  },
  {
    path: 'leaderboard',
    component: LeaderboardViewComponent,
  },
  {
    path: 'game',
    component: GameViewComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

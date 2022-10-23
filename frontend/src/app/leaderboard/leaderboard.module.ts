import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeaderboardViewComponent } from './leaderboard-view.component';
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    LeaderboardViewComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    // Material
    MatTableModule,
  ],
  exports: [
    LeaderboardViewComponent,
  ]
})
export class LeaderboardModule { }

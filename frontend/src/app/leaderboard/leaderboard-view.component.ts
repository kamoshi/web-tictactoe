import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {ApiService} from "../api.service";

@Component({
  selector: 'app-leaderboard-view',
  template: `
    <table mat-table [dataSource]="leaderboard" class="mat-elevation-z8">
      <!-- Player Column -->
      <ng-container matColumnDef="player">
        <th mat-header-cell *matHeaderCellDef> Player </th>
        <td mat-cell *matCellDef="let element">{{ element.name }}</td>
      </ng-container>

      <!-- Wins Column -->
      <ng-container matColumnDef="wins">
        <th mat-header-cell *matHeaderCellDef> Wins </th>
        <td mat-cell *matCellDef="let element">{{ element.wins }}</td>
      </ng-container>

      <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
      <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
    </table>
  `,
  styles: [`
    table {
      margin: 2em auto;
      min-width: 50vw;
    }
  `]
})
export class LeaderboardViewComponent implements OnInit {
  readonly displayedColumns = ['player', 'wins'];
  leaderboard: LeaderboardDto[] = [];

  constructor(
    private api: ApiService,
  ) { }

  ngOnInit(): void {
    this.api.getLeaderboard().subscribe({
      next: value => this.leaderboard = value,
      error: err => console.error(err),
    });
  }

}

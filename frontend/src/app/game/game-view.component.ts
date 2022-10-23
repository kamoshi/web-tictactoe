import {Component, OnDestroy, OnInit} from '@angular/core';
import { IdentityService } from "../identity.service";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { interval, Subscription } from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {GameEndDialogComponent} from "./game-end-dialog.component";
import {ApiService} from "../api.service";

@Component({
  selector: 'app-game-view',
  template: `
    <div class="game" *ngIf="data; else noGame">
      <mat-card>
        <mat-card-title>Board</mat-card-title>
        <div class="board">
          <div class="board-row" *ngFor="let row of data.board; let r = index">
            <button class="board-cell" *ngFor="let cell of row; let c = index" (click)="onMove(r, c)">
                {{cell}}
            </button>
          </div>
        </div>
      </mat-card>

      <div>
        <div>
          <span class="info-label">Player O:</span>
          {{ data.playerO }}
        </div>
        <div>
          <span class="info-label">Player X:</span>
          {{ data.playerX }}
        </div>
        <div>
          <span class="info-label">Current move:</span>
          {{ move }}
        </div>
        <div *ngIf="data.state">
          <span class="info-label">Result:</span>
          {{ state }}
        </div>
      </div>
    </div>

    <ng-template #noGame>
      <div class="no-game">
        <h1>Creating a game for you...</h1>
        <mat-spinner></mat-spinner>
      </div>
    </ng-template>
  `,
  styles: [`
    .game {
      display: flex;
      margin: 2em auto;
      justify-content: center;
      gap: 2em;
    }
    .info-label {
      font-weight: 700;
    }
    .board-row {
      display: flex;
    }
    .board-cell {
      min-width: 5em;
      min-height: 5em;
    }
    .no-game {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;

      h1 {
        margin: 2em auto;
      }
    }
  `]
})
export class GameViewComponent implements OnInit, OnDestroy {
  private gameId?: number;
  private _refresher?: Subscription;
  private _modalOpened = false;
  data?: GameDto;

  constructor(
    private http: HttpClient,
    private router: Router,
    public identity: IdentityService,
    public dialog: MatDialog,
    private api: ApiService,
  ) {}

  openDialog() {
    if (this._modalOpened) return;
    this._modalOpened = true;
    this.dialog.open(GameEndDialogComponent, { width: '250px' })
      .afterClosed().subscribe(next => {
        this.joinGame();
        this._modalOpened = false;
      });
  }

  get move(): string {
    switch (this.data?.move) {
      case 0: return 'O';
      case 1: return 'X';
      default: return '';
    }
  }

  get state(): string {
    switch (this.data?.state) {
      case 0: return 'Ongoing';
      case 1: return 'Tied';
      case 2: return 'Player O has won';
      case 3: return 'Player X has won';
      default: return '';
    }
  }

  async ngOnInit() {
    if (!this.identity.isLoggedIn)
      await this.router.navigate(['login']);

    this.joinGame();

    this._refresher = interval(1000).subscribe(x => {
      if (!this.gameId) return;

      if (this.data !== undefined && this.data.state !== 0)
        this.openDialog();

      this.api.getGame(this.gameId).subscribe({
        next: data => this.data = data,
        error: err => console.error(err),
      });
    })
  }

  ngOnDestroy() {
    this._refresher?.unsubscribe();
  }

  onMove(row: number, col: number) {
    if (!this.gameId || !this.identity.id)
      return;

    this.api.move(this.gameId, this.identity.id, row, col).subscribe({
        next: value => this.data = value,
        error: err => console.error(err),
      })
  }

  private joinGame() {
    if (!this.identity.id)
      return;

    this.api.joinGame(this.identity.id).subscribe({
      next: gameId => this.gameId = gameId,
      error: err => console.error(err)
    });
  }
}

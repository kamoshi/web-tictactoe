import { Component } from "@angular/core";
import {MatDialogRef} from "@angular/material/dialog";


@Component({
  selector: 'game-end-dialog',
  template: `
    <h1 mat-dialog-title>Game has ended!</h1>
    <div mat-dialog-actions>
      <button mat-button mat-dialog-close cdkFocusInitial>Ok</button>
    </div>
  `
})
export class GameEndDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<GameEndDialogComponent>
  ) {}
}

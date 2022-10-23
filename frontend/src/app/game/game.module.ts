import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameViewComponent } from './game-view.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { GameEndDialogComponent } from "./game-end-dialog.component";
import {MatButtonModule} from '@angular/material/button';

@NgModule({
  declarations: [
    GameViewComponent,
    GameEndDialogComponent,
  ],
  imports: [
    CommonModule,
    // Material
    MatProgressSpinnerModule,
    MatCardModule,
    MatDialogModule,
    MatButtonModule,
  ]
})
export class GameModule { }

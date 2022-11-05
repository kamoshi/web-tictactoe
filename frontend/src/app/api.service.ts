import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly address = 'http://localhost:7135/api';
  private readonly endpoints = {
    login: 'Auth',
    leaderboard: 'Players/leaderboard',
    game: 'Games',
    joinGame: 'Games/join',
    move: 'Games',
  } as const;

  constructor(
    private http: HttpClient,
  ) {
    console.log('TEST');
  }

  getLeaderboard() {
    return this.http.get<LeaderboardDto[]>(`${this.address}/${this.endpoints.leaderboard}`);
  }

  login(name: string) {
    return this.http.post<number>(`${this.address}/${this.endpoints.login}`, { name });
  }

  getGame(gameId: number) {
    return this.http.get<GameDto>(`${this.address}/${this.endpoints.game}/${gameId}`)
  }

  joinGame(playerId: number) {
    return this.http.post<number>(`${this.address}/${this.endpoints.joinGame}`, { playerId });
  }

  move(gameId: number, playerId: number, row: number, col: number) {
    return this.http.put<GameDto>(`${this.address}/${this.endpoints.move}/${gameId}`, { playerId, row, col});
  }
}

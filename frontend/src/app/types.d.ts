interface Identity {
  id: number;
  name: string;
}

interface GameDto {
  playerO: string;
  playerX: string;
  board: string[][];
  move:
    | 0   // O
    | 1;  // X
  state:
    | 0   // None
    | 1   // WinO
    | 2   // WinX
    | 3;  // Tied
}

interface LeaderboardDto {
  name?: string;
  wins: number;
}

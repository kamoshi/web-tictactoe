import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  private readonly lsKey = 'IdentityService';

  save(identity: Identity) {
    const json = JSON.stringify(identity);
    localStorage.setItem(this.lsKey, json);
  }

  load(): Identity | null {
    const json = localStorage.getItem(this.lsKey);
    return json ? JSON.parse(json) : null;
  }
}

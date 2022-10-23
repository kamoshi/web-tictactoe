import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  private readonly lsKey = 'IdentityService';
  private _identity: Identity | null | undefined;

  get isLoggedIn() { return !!this._identity }
  get id() { return this._identity?.id }
  get name() { return this._identity?.name }

  constructor() {
    this.load();
  }

  load(): Identity | undefined {
    if (this._identity == undefined)
      this._identity = this._load();
    return this._identity ?? undefined;
  }

  save(identity: Identity) {
    this._identity = { ...identity };
    localStorage.setItem(this.lsKey, JSON.stringify(this._identity));
  }

  reset() {
    this._identity = null;
    localStorage.removeItem(this.lsKey);
  }

  private _load(): Identity | null {
    const json = localStorage.getItem(this.lsKey)
    return json ? JSON.parse(json) as Identity : null;
  }
}

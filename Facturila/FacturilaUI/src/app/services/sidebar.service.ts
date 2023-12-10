import { Injectable } from '@angular/core';
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class SidebarService {

  constructor() { }

  private _sidebarVisible = new BehaviorSubject<boolean>(true);
  sidebarVisible = this._sidebarVisible.asObservable();

  toggleSidebar() {
    this._sidebarVisible.next(!this._sidebarVisible.value);
  }
}

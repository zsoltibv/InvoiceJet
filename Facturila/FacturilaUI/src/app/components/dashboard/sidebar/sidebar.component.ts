import { Component } from '@angular/core';
import { FormControl, FormGroup } from "@angular/forms";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  searchForm = new FormGroup({
    search: new FormControl(''),
  });

  mainItems: any[] = [
    {
      id: 1,
      name: "Factura",
      routerLink: "/dashboard/buttons",
      active: true
    }, {
      id: 2,
      name: "Accordions",
      routerLink: "/dashboard/accordions",
      active: true
    },
  ];

  tmpItems: any[] = []
}

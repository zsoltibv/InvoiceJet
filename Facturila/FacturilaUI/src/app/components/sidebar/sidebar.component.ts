import { Component } from '@angular/core';
import { TreeNode } from "primeng/api";
import { Subscription } from "rxjs";
import { SidebarService } from "src/app/services/sidebar.service";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {
  files: TreeNode[];

  sidebarVisible = true;
  private subscription: Subscription;

  constructor(private sidebarService: SidebarService) {
    this.subscription = this.sidebarService.sidebarVisible.subscribe(
      (visible) => (this.sidebarVisible = visible)
    );

    this.files = [
      {
        label: 'Emitere',
        icon: 'pi pi-inbox',
        children: [
          {
            label: 'Factura',
          },
          {
            label: 'Factura Storno',
          },
          {
            label: 'Proforma',
          }
        ]
      },
      {
        label: 'Rapoarte',
        icon: 'pi pi-chart-bar',
        children: [
          {
            label: 'Rapoarte Facturi',
          },
          {
            label: 'Rapoarte Proforme',
          },
          {
            label: 'Proforma',
          }
        ]
      },
      {
        label: 'Setari',
        icon: 'pi pi-cog',
        children: [
          {
            label: 'Date Generale',
            children: [
              {
                label: 'Date Cont',
              },
              {
                label: 'Date Firma',
              },
              {
                label: 'Conturi Bancare',
              },
            ]
          },
          {
            label: 'Nomenclatoare',
            children: [
              {
                label: 'Produse',
              },
              {
                label: 'Clienti',
              },
            ]
          },
        ]
      }
    ];
  }


}

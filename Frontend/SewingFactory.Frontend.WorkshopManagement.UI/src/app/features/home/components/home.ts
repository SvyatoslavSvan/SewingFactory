import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {NgForOf} from '@angular/common';

interface NavItem {
  title: string;
  link: string;
  iconClass: string;
}

@Component({
  selector: 'app-home', imports: [RouterLink, NgForOf],
  templateUrl: '../pages/home.html',
  styleUrl: '../pages/home.scss'
})
export class Home {
  navItems: NavItem[] = [
    { title: 'Відділи',       link: '/departments',      iconClass: 'bi bi-building'       },
    { title: 'Працівники',    link: '/employees',        iconClass: 'bi bi-people'         },
    { title: 'Категорії одежи',link: '/categories',       iconClass: 'bi bi-tags'           },
    { title: 'Моделі одежи',  link: '/garment-models',   iconClass: 'bi bi-person-badge'   },
    { title: 'Документи',     link: '/documents',        iconClass: 'bi bi-file-earmark'   },
    { title: 'Табель',        link: '/timesheet',        iconClass: 'bi bi-clock-history'  },
    { title: 'Процеси',       link: '/process',          iconClass: 'bi bi-gear'           }
  ];
  constructor(private router: Router) {

  }
  logout(): void {
    // ваша логика выхода
    // Например: this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}

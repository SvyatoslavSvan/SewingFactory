import {Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {Navbar} from './core/layout/navbar/navbar';

@Component({
  standalone: true,
  selector: 'app-root', imports: [RouterOutlet, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected title = 'SewingFactory.Frontend.WorkshopManagement.UI';
}

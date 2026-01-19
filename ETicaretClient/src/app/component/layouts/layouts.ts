import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CartsService } from '../../services//carts.service';

@Component({
  selector: 'app-layouts',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './layouts.html',
  styleUrl: './layouts.css',
})
export class Layouts {
  constructor(
      public cart: CartsService
    ){}
}

import { Component, signal } from '@angular/core';
import { FlexiGridModule } from 'flexi-grid';
import { TrCurrencyPipe } from 'tr-currency';
import { OrderModel } from '../../models/order.model';
import { HttpClient } from '@angular/common/http';
import { ResultModel } from '../../models/result.model';
import { api } from '../../constants';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [FlexiGridModule, TrCurrencyPipe],
  templateUrl: './orders.html',
  styleUrl: './orders.css'
})
export class Orders {
  orders = signal<OrderModel[]>([]);
  token = signal<string>("");
  
  constructor(
    private http: HttpClient    
  ) {
    if(localStorage.getItem("my-token")){
      this.token.set(localStorage.getItem("my-token")!);

      this.getAll();
    }
  }

  getAll(){
    this.http.get<ResultModel<OrderModel[]>>(`${api}/orders/getall`,{
      headers: {
        "Authorization": "Bearer " + this.token()
      }
    }).subscribe(res=> {
      this.orders.set(res.data!);
    });
  }
}
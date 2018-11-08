import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AdalService } from 'adal-angular4';

@Component({
  selector: 'app-booktest',
  templateUrl: './booktest.component.html',
  styleUrls: ['./booktest.component.css'],  
})
export class BooktestComponent implements OnInit {
  bookId: string = '1';
  resText: string = '';  

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private adalService: AdalService) {}

  ngOnInit() {
  }  

  onSubmit(text: string, ): void {
    // JWT to header    
    const httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')      
      .set('Authorization', 'Bearer ' + this.adalService.userInfo.token);      
    // call API
    this.http.get<string>(this.baseUrl + 'api/books/' + this.bookId, { responseType: 'text' as 'json', headers: httpHeaders }).subscribe(result => {
      this.resText = result;
      console.log("retrieved" + result);      
    }, error => console.error(error));
  }
}

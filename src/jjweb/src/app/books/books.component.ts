import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AdalService } from 'adal-angular4';

import {Book} from '../book';
import {BOOKS} from '../mock-books';
import { Options } from 'selenium-webdriver/firefox';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit { 
  //books = BOOKS;
  books: Book[];
  selectedBook: Book;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private adalService: AdalService) {
    // JWT to header    
    const httpHeaders = new HttpHeaders()
      .set('Content-Type', 'application/json')      
      .set('Authorization', 'Bearer ' + adalService.userInfo.token);      
    // call API
    http.get<Book[]>(baseUrl + 'api/books', { headers: httpHeaders }).subscribe(result => {
      this.books = result;
      console.log("retrieved" + this.books);
      console.log("title: " + this.books[0].name);
    }, error => console.error(error));
  }

  ngOnInit() {
  }

  onSelect(book: Book): void {
    this.selectedBook = book;
  }
}

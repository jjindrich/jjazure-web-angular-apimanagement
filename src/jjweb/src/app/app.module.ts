import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BooksComponent } from './books/books.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MaterialModule } from './material/material.module';

import { AdalService, AdalGuard, AdalInterceptor } from 'adal-angular4';
import { BooktestComponent } from './booktest/booktest.component';

@NgModule({
  declarations: [
    AppComponent,
    BooksComponent,
    ToolbarComponent,
    NotFoundComponent,
    BooktestComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MaterialModule
  ],
  providers: [AdalService, AdalGuard, { provide: HTTP_INTERCEPTORS, useClass: AdalInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }

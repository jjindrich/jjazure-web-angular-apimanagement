import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BooktestComponent } from './booktest.component';

describe('BooktestComponent', () => {
  let component: BooktestComponent;
  let fixture: ComponentFixture<BooktestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BooktestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BooktestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

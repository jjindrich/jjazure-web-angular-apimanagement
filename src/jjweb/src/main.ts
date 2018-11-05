import 'hammerjs';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

const providers = [
  // API App (Web App)
  //{ provide: 'BASE_URL', useValue: 'https://jjapiapp.azurewebsites.net/', deps: [] }

  // API management with no authorization
  //{ provide: 'BASE_URL', useValue: 'https://jjapi.azure-api.net/books/', deps: [] }

  // API management with authorization enabled
  { provide: 'BASE_URL', useValue: 'https://jjapi.azure-api.net/BooksSecure/', deps: [] }  
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.error(err));

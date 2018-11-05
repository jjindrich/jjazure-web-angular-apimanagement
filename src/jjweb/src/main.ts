import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

const providers = [
  //{ provide: 'BASE_URL', useValue: 'https://jjapiapp.azurewebsites.net/', deps: [] }
  { provide: 'BASE_URL', useValue: 'https://jjapi.azure-api.net/books/', deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.error(err));

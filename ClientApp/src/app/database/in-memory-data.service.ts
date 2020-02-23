import { Injectable } from '@angular/core';

import { InMemoryDbService } from 'angular-in-memory-web-api';

@Injectable({
  providedIn: 'root'
})
export class InMemoryDataService implements InMemoryDbService {
  createDb() {

    const products = [
      {
        id: '1',
        code: 'Quinn',
        name: 'Nixon',
        photo: '22222222222222222222222',
        price: 20.5,
        lastUpdated: '10.20.2018'
      },
      {
        id: '2',
        code: 'Eric',
        name: 'Smith',
        photo: '22222222222222222222222',
        price: 10.5,
        lastUpdated: '10.20.2018'
      },
      {
        id: '3',
        code: 'Carlson',
        name: 'Cox',
        photo: '22222222222222222222222',
        price: 14,
        lastUpdated: '10.20.2018'
      }, {
        id: '4',
        code: 'Kelsea',
        name: 'Kelly',
        photo: '22222222222222222222222',
        price: 23.9,
        lastUpdated: '10.20.2018'
      }, {
        id: '5',
        code: 'Aino',
        name: 'Uno',
        photo: '22222222222222222222222',
        price: 78.3,
        lastUpdated: '10.20.2018'
      }, {
        id: '6',
        code: 'Amy',
        name: 'Little',
        photo: '22222222222222222222222',
        price: 22.54,
        lastUpdated: '10.20.2018'
      }, {
        id: '7',
        code: 'Doris',
        name: 'Chandler',
        photo: '22222222222222222222222',
        price: 77.33,
        lastUpdated: '10.20.2018'
      }
    ];

    return { products };
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Summary } from '../../models/summary.model';

@Injectable({
  providedIn: 'root'
})
export class SummaryService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiBaseUrl}/api/summary`;

  getSummary(from?: string, to?: string): Observable<Summary> {
    let params = new HttpParams();

    if (from) {
      params = params.set('from', from);
    }

    if (to) {
      params = params.set('to', to);
    }

    return this.http.get<Summary>(this.endpoint, { params });
  }
}

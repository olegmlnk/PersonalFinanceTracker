import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  Transaction,
  TransactionFilters,
  TransactionUpsertRequest
} from '../../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = `${environment.apiBaseUrl}/api/transactions`;

  getTransactions(filters?: TransactionFilters): Observable<Transaction[]> {
    let params = new HttpParams();

    if (filters?.from) {
      params = params.set('from', filters.from);
    }

    if (filters?.to) {
      params = params.set('to', filters.to);
    }

    if (filters?.type) {
      params = params.set('type', filters.type);
    }

    return this.http.get<Transaction[]>(this.endpoint, { params });
  }

  createTransaction(payload: TransactionUpsertRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.endpoint, payload);
  }

  updateTransaction(id: number, payload: TransactionUpsertRequest): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.endpoint}/${id}`, payload);
  }

  deleteTransaction(id: number): Observable<void> {
    return this.http.delete<void>(`${this.endpoint}/${id}`);
  }
}

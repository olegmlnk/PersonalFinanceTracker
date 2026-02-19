import type { TransactionType } from './transaction.model';

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
}

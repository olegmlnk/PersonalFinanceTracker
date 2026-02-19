import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { catchError, of } from 'rxjs';
import { SummaryService } from '../../core/services/summary.service';
import { Summary } from '../../models/summary.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  private readonly summaryService = inject(SummaryService);

  protected readonly summary$ = this.summaryService.getSummary().pipe(
    catchError(() =>
      of<Summary>({
        totalIncome: 0,
        totalExpense: 0,
        balance: 0
      })
    )
  );
}

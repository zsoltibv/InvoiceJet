import { Injectable } from "@angular/core";
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private toastr: ToastrService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        const serverError = error.error;
        let errorMessage = "Unknown Error Occurred";

        if (serverError instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Error: ${serverError.message}`;
        } else {
          // Server-side error
          errorMessage =
            serverError?.message || error.message || "Server error";
          this.handleError(error.status, errorMessage);
        }

        return throwError(() => new Error(errorMessage));
      })
    );
  }

  private handleError(statusCode: number, message: string): void {
    console.error("Final error message:", message);
    switch (statusCode) {
      case 400:
        this.toastr.error(message, "Error");
        break;
      case 401:
        this.toastr.error("Session has expired", "Unauthorized");
        break;
      case 404:
        this.toastr.error(message, "Not Found");
        break;
      case 500:
        this.toastr.error(message, "Internal Server Error");
        break;
      default:
        this.toastr.error(message, "Unexpected Error");
        break;
    }
  }
}
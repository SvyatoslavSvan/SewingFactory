import {
  HttpInterceptorFn
} from '@angular/common/http';

export const baseUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = 'https://localhost:20002';
  const url = req.url.startsWith('http') ? req.url : `${baseUrl}${req.url}`;
  return next(req.clone({ url }));
};

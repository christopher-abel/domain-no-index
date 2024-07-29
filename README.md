# domain-no-index

Small MVC project to help purge any urls from SEO indexing.

A catch-all route handles all URLs in a single controller action:
- returns a 404 status code
- returns a no-index meta tag

Robots.txt
- robots.txt with `Disallow: /`

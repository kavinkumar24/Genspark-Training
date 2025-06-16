# Day 31

## Topics

- Debounce - make an optimized search
- Distinctunitchanged
- Tap
- SwitchMap
- Hostlistener
- limit, skip with infinte scrolling
- set a threshold level of the page
- Routing in a Single page application


## Debounce
- It is a Rxjs observable, which process the input request with certain time period, so instead of make every time request, it is like a sceduled request 

## DistinctUntilChanged 
- If same input occurs with in that time period then it make that call nullify returns the previous result for that input

## Tap
- Used to perform side effects without modifiying the original data


## SwitchMap
 - Cancels the previous observable and switches to a new one when a new valus is emitted

 ## Host Listener

 - Listens the DOM events on the component or directive's host 


 ### Task 
 Objective:
Create a simple Angular application that lets users browse and search for products using infinite scroll and debounce-based search. The app will have basic routing with two routes: Home and About.
 Requirements:
1. Routing Setup
•	Implement basic routing with two routes:
o	/home → displays the product listing
o	/about → static page with dummy text (e.g., "This is a demo app built using Angular")
 
2. Product Listing Page (/home)
•	Fetch data from the DummyJSON API:
o	URL: https://dummyjson.com/products/search?q=<searchTerm>&limit=10&skip=<skip>
•	Show product cards with:
o	Product titlem
o	Thumbnail image
o	Price
 
3. Debounce Search
•	Add a search input with debounce (400ms) using RxJS.
•	On search, call the API with the query and reset pagination.
 
4. Infinite Scroll
•	Implement infinite scroll by listening to the scroll event.
•	Load more products when the user nears the bottom of the list.
•	Use the skip parameter for pagination.
 
5. Loading Indicator
•	Show a loader while fetching data.
 
Bonus (Optional)
•	Add a "Back to Top" button.
•	Highlight search term in product titles.

# eShop Microservices
This is a simple shopping web that use docker to build microservices.

## User stories
1. Users should be able to select a product from the catalog listing
2. Users should be able to add a product to the basket
3. Users should be able to remove a product from the basket
4. Users should be able to get the best price of product by decreasing the discount when they are at the checkout page
5. Users should be able to checkout the basket that will create an order and remove the basket

## How to run
1. Clone the repo
2. Install the docker
3. Go to the `src` folder
4. Run `docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build`
5. The webapp should be up at: `http://host.docker.internal:8006/`
# Hotels API with rate limiting

##Rate Limiting
This api limits N requests for a particular api key per X Period of time. If the number of request exceeds the threshold, that key is suspended for 5 minutes (configurable).
The apikey is sent as the request header with the key "Key". Requests without a key will cause an error. 

Projet created for agoda.com

##Usage: 
api/hotels/city/{city}

##Sorting:
api/hotels/city/{city}?pricesort={asc|desc}




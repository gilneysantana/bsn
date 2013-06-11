from bottle import Bottle, route, run, template, debug, get, post, request, response, hook

@hook('after_request')
def enableCORSAfterRequestHook():
    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Access-Control-Allow-Headers'] = 'X-Requested-With'
 

@route('/phones', method=['GET', 'OPTIONS'])
def api_status():
    return '''[ {
        "age": 0, 
        "id": "motorola-xoom-with-wi-fi", 
        "imageUrl": "img/phones/i.0.jpg", 
        "name": "Motorola XOOM with Wi-Fi", 
        "snippet": "The Next, Next gilney tablet powered by Android 3.0 (Honeycomb)."
    }, {
        "age": 1, 
        "id": "Manuella Machado", 
        "imageUrl": "img/phones/i.0.jpg", 
        "name": "Motorola XOOM with Wi-Fi", 
        "snippet": "Manuuu"
    }, {
        "age": 2, 
        "id": "Gilney Santana", 
        "imageUrl": "img/phones/i.0.jpg", 
        "name": "Motorola XOOM with Wi-Fi", 
        "snippet": "Gilney Santana"
    }]'''
    

debug(True)
run(host='localhost', port=8888, debug=True, reloader=True)


from bottle import Bottle, route, run, template, debug, get, post, request, response, hook
import banco
import json 

@hook('after_request')
def enableCORSAfterRequestHook():
    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Access-Control-Allow-Headers'] = 'Origin, Accept, Content-Type, X-Requested-With, X-CSRF-Token'
    response.headers['Access-Control-Allow-Methods'] = 'POST, GET, OPTIONS, PUT, DELETE'
 
#########################

@route('/alvos', method=['GET', 'OPTIONS'])
def api_alvos():
	return banco.alvos()
    
#########################

@route('/anuncios', method=['GET', 'OPTIONS'])
def api_anuncios():
	return banco.anuncios()

#########################

@route('/sites', method=['GET', 'OPTIONS'])
def sites_list():
	return banco.sites()
    
#@route('/sites/<name>', method=['GET', 'OPTIONS'])
#def site_show( name="Nao informado"):
#	return "Show site " + name
#    
#@route('/sites/<name>', method=['DELETE', 'OPTIONS'])
#def site_delete( name="Nao informado"):
#	return "DELETE site " + name
    
@route('/sites/novo', method=['POST', 'OPTIONS'])
def site_delete():
    data = request.body.readline()
    if not data:
        return "400, 'No data received'"
	
    return "POST - idade=" + request.forms.idade

#########################

debug(True)
run(host='localhost', port=8888, debug=True, reloader=True)


from bottle import Bottle, route, run, template, debug, get, post, request, response, hook
import banco
import json
import buscador

@hook('after_request')
def enableCORSAfterRequestHook():
    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Access-Control-Allow-Headers'] = 'Origin, Accept, Content-Type, X-Requested-With, X-CSRF-Token'
    response.headers['Access-Control-Allow-Methods'] = 'POST, GET, OPTIONS, PUT, DELETE'
 
#########################

@route('/alvos', method=['GET', 'OPTIONS'])
def api_alvos():
	return banco.alvos()
    
@route('/alvos/<site_id>', method=['GET', 'OPTIONS'])
def api_alvo(site_id):
	site, id_ = site_id.split("-")
	return banco.alvo(site, id_)
    
@route('/alvos/<site_id>', method=['PUT', 'OPTIONS'])
def api_alvo_update(site_id):
	data = request.body.readline()
	if not data:
	    return "400, 'No data received'"
	site, id_ = site_id.split("-")
	buscador.atualizarHtml(site, id_)
	return "Status do Alvo({0}) alterado para {1}" \
		.format(site_id, str(request.json['novoStatusAlvo']))
    
#########################

@route('/anuncios', method=['GET', 'OPTIONS'])
def api_anuncios():
	return banco.anuncios()

#########################

@route('/sites', method=['GET', 'OPTIONS'])
def sites_list():
	return banco.sites()
    
@route('/sites/novo', method=['POST', 'OPTIONS'])
def site_novo():
    data = request.body.readline()
    if not data:
        return "400, 'No data received'"
	
    return "No servidor" + str(request.json)

#########################

debug(True)
run(host='0.0.0.0', port=8888, debug=True, reloader=True)


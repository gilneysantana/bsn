import sqlite3
import json

def alvos():
	conn = sqlite3.connect('cashew.sqlite')
	c = conn.cursor()
	retorno = c.execute('select * from alvo')
	dados_json = json.dumps(retorno.fetchall())
	c.close()
	return dados_json

def alvo(site, id_):
	conn = sqlite3.connect('cashew.sqlite')
	c = conn.cursor()
	retorno = c.execute("select * from alvo where siteOrigem =%(nomeSite)s and id=%(idAlvo)s" % {"nomeSite":site, "idAlvo":id_})
	dados_json = json.dumps(retorno.fetchall())
	c.close()
	return dados_json

def anuncios():
	conn = sqlite3.connect('cashew.sqlite')
	c = conn.cursor()
	retorno = c.execute('select * from anuncio')
	dados_json = json.dumps(retorno.fetchall())
	c.close()
	return dados_json

def sites():
	conn = sqlite3.connect('cashew.sqlite')
	c = conn.cursor()
	retorno = c.execute('select * from site')
	dados_json = json.dumps(retorno.fetchall())
	c.close()
	return dados_json

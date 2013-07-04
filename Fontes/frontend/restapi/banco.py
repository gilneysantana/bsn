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
	retorno = c.execute("select * from alvo where site_cd_site =%(nomeSite)s and alvo_cd_alvo =%(idAlvo)s" % {"nomeSite":site, "idAlvo":id_})
	dados_json = json.dumps(retorno.fetchall())
	c.close()
	return dados_json

def alvoRow(site, id_):
	conn = sqlite3.connect('cashew.sqlite')
	conn.row_factory = sqlite3.Row
	cur = conn.cursor()
	retorno = cur.execute("select * from alvo where site_cd_site=%(nomeSite)s and alvo_cd_alvo=%(idAlvo)s" % {"nomeSite":site, "idAlvo":id_})
	return retorno.fetchone();

def alvoUpdate(alvo):
	conn = sqlite3.connect('cashew.sqlite')
	c = conn.cursor()
	sql = "update alvo set retornoRequisicao = 'agora vai' where site_cd_site = 2 and alvo_cd_alvo = 1233"
	retorno = c.execute(sql)
	conn.commit()
	c.close()

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

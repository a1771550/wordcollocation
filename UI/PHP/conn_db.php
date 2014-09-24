<?php
/**
 * Created by IntelliJ IDEA.
 * User: Kevin Lau
 * Date: 09/24/2014
 * Time: 12:00
 */
require_once 'login.php';
$db_server = mysql_connect($db_hostname, $db_username, $db_password);
if(!$db_server) die("Unable to connect to MySql: " . mysql_errno());

mysql_select_db($db_database) or die("Unable to select database: " . $mysql_error());
